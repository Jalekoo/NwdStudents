(function () {
    var Acw = window.Acw;
    Acw.VM = window.Acw.VM = window.Acw.VM || {};

    var _setDefaultValues = function (list) {
        list.Count("0");
        list.CanGoNext(false);
        list.CanGoPrev(false);
        list.Move(0);
        list.MemoryKey("");
        list.PagerMemory("");
    };

    Acw.VM.Filters = Acw.ViewModel.extend({
        init: function (collection, model) {
            this._super( null, null, model);
            var t;
            this.onPropertyChanged(function (value) {
                clearTimeout(t);
                t = setTimeout(function () {
                    collection.reload();
                }, 500);
            });
        },

        onPropertyChanged: function (onPropertyChanged) {
            Acw.VM.onPropertyChanged(this, onPropertyChanged);
        }
    });

    /*
    *   PagedCollection CK.Search ViewModel compliant !
    *   viewModel: Acw.ViewModel. The VM of the item collection
    *   url: The query url
    *   [dataModel] :  The initial model
    */
    Acw.VM.PagedCollection = Acw.ViewModel.extend({
        init: function (viewModel, url, options) {
            options = options || {};
            var dataModel = options.dataModel || {};
            var filterModel = options.filterModel || Acw.VM.Filters,
                filterDataModel = options.filterDataModel || null;

            var _this = this;

            this.PerPage = ko.observable(10);
            this.Count = ko.observable("0");

            this.Filters = new filterModel(this, filterDataModel);

            this.Sorters = ko.observableArray([]);
            this.CanGoNext = ko.observable(false);
            this.CanGoPrev = ko.observable(false);
            this.Move = ko.observable(0);
            this.MemoryKey = ko.observable("");
            this.PagerMemory = ko.observable("");

            this.Items = ko.observableArray([]);

            this._super({ url: url, reverseMode: false }, '', dataModel);
            
            this.itemVM = viewModel;
            this.IsFetching = ko.observable(false);
            this.FetchingFailure = ko.observable(false);

            this.PerPage.subscribe(function (newVal) {
                _this.Move(0);
                _this.MemoryKey("");
                _this.PagerMemory("");
            });
        },
        mappingOptions: function () {
            var _this = this;
            return {
                Items: (_this.hasId() ? {
                    create: function (options) {
                        return new _this.itemVM(options.data);
                    },
                    key: function (data) {
                        return ko.utils.unwrapObservable(data[new _this.itemVM().idPropertyName]);
                    }
                } : {
                    create: function (options) {
                        return new _this.itemVM(options.data);
                    }
                })
            };
        },
        next: function () {
            this.Move(1);
            if (this.CanGoNext()) this.fetch();
        },
        more: function () {
            this.Move(2);
            if (this.CanGoNext()) this.fetch();
        },
        prev: function () {
            this.Move(-1);
            if (this.CanGoPrev()) this.fetch();
        },

        //Override

        reconfigure: function( settings ){
            this.settings = settings;
            this.reset();
        },

        reload: function (callback) {
            this.reset();
            this.fetch(callback);
        },

        reset: function(){
            this.clearItems();
            _setDefaultValues(this);
        },

        clearItems: function(){
            this.Items.removeAll();
        },

        //Override
        fetch: function (callback) {
            var _this = this;
            if (!this.settings.url) return;

            this.FetchingFailure(false);
            this.IsFetching(true);
            function direction(){
                if (_this.Move() < 0) return '/prev';
                else if (_this.Move() == 0) return '';
                else return '/next';     
            }
            Acw.ViewModel.request({
                'url': _this.settings.url + direction(),
                useGet: true, //_this.Move() == 0,
                parameters: {
                    perPage: _this.PerPage,
                    filters: _this.Filters,
                    sorters: _this.Sorters,
                    pagerMemory: _this.PagerMemory,
                    memoryKey: _this.MemoryKey
                }
            }, function (data) {    //Success
                var more;
                
                if (_this.Move() === 2)
                {
                    more = ko.mapping.fromJS(data, _this.mappingOptions());
                    if (typeof more !== 'undefined') {
                        $.each(more.Items(), function (idx, e) {
                            _this.settings.reverseMode === false ? _this.Items.push(e) : _this.Items.unshift(e);
                        });
                    }
                    _this.map(data, { ignore: ["Items"] });
                }
                else {
                    ko.mapping.fromJS(data, _this.mappingOptions(), _this);
                }
                
                if(typeof callback === 'function') callback();
                _this.IsFetching(false);

            }, function () {    //Error

                _this.IsFetching(false);
                _this.FetchingFailure(true);
                setTimeout(function () {
                    _this.FetchingFailure(false);
                }, 10000);

            });
        }
    });
   
})();
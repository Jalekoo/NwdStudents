(function () {
    var SO = window.SO;
    var _setDefaultValues = function (list) {
        list.Count("0");
        list.CanGoNext(false);
        list.CanGoPrev(false);
        list.Move(0);
        list.MemoryKey("");
        list.PagerMemory("");
    };

    SO.VM.Filters = SO.ViewModel.extend({
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
            SO.VM.onPropertyChanged(this, onPropertyChanged);
        }
    });

    /*
    *   PagedCollection CK.Search ViewModel compliant !
    *   viewModel: SO.ViewModel. The VM of the item collection
    *   url: The query url
    *   [dataModel] :  The initial model
    */
    SO.VM.Collection = SO.ViewModel.extend({
        init: function (viewModel, url, options) {
            options = options || {};
            var _this = this;

            this.Items = ko.observableArray([]);
            this.IsFetching = ko.observable(false);
            this.FetchingFailure = ko.observable(false);
            this._super({ url: url });
            
            this.itemVM = viewModel;
            
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

            _this.FetchingFailure(false);
            _this.IsFetching(true);

            SO.ViewModel.request({
                'url': _this.settings.url,
                useGet: true, //_this.Move() == 0,
                parameters: {}
            }, function (data) {    //Success
                ko.mapping.fromJS({ Items : data }, _this.mappingOptions(), _this);

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
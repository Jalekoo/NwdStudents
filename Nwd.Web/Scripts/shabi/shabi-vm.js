(function () {

    var SO = window.SO;

    /**
     *   The model Name
     *   
     **/
    SO.ViewModel = ViewModel.extend({
        init: function (urlParameters,model, idPropertyName) {
            this.idPropertyName = idPropertyName || '';

            this.settings = $.extend({}, { url: '', parameters: {}, useGet: true, async: true }, urlParameters);

            if (typeof model === 'undefined') {
                this.fetch();
            }
            else {
                this.map(model);
            }
        },
        id: function(){
            return ko.utils.unwrapObservable(this[this.idPropertyName]);
        },
        hasId: function(){
            return typeof this.id() !== 'undefined';
        },

        /**
        *   Map the model to te VM
        *   model: the model object to map
        *   [options] : { include: Array, ignore: Array, propertyName: { key: function(data), create: function(options) } }
        **/
        map: function (model, options) {
            if (typeof model !== 'object') return;

            options = options || {};
            return ko.mapping.fromJS(model, options, this);
        },

        fetch: function (parameters, success, failure) {
            var _this = this;
            SO.ViewModel.request((typeof parameters === 'object' ? parameters : _this.settings), function (data) {
                _this.map(data);
                if (typeof success === "function") {
                    success(data);
                }
            }, function () {
                if (typeof failure === "function") {
                    failure(data);
                }
            });
        },

        /**
        *   Reset de VM with the default values.
        */
        clear: function () {
            SO.VM.unobserve(this);
            var vm = ViewModel.extend(this);
            var defaultModel = SO.VM.unwrap(new vm());
            ko.mapping.fromJS(defaultModel, {}, this);
        },

        /**
        *   Get or set the model. If the value passed is a VM then it's automaticaly observed with the observe() method.
        *   value: Object. The new model.
        *   urlParameters. The new url settings.
        **/
        model: function (value, urlParameters) {
            var _this = this;
            if (typeof value === 'undefined') return SO.VM.unwrap(_this);

            this.clear();
            if (value instanceof ViewModel) SO.VM.observe(this, value);
            else this.map(value);
                
            this.settings = $.extend({}, { url: '', parameters: {}, useGet: true, async: true, method: '' }, urlParameters);
        }
    });
    
    /**
    *   Submit a post or a get HTML request
    *   options: { url: string , parameters: object, useGet: boolean, async: boolean }. Note that parameters can be observable
    *   success: function(data)
    *   failure: function()
    */
    SO.ViewModel.request = function (options, success, failure) {
        options = $.extend({}, { url: '', parameters: {}, useGet: true, async: true, method: 'post' }, options);
        if(!options.url) return;
        if (options.url[0] != '/') options.url = SO.rootUrl + options.url;

        if (options.useGet) $.get(options.url, SO.VM.unwrap(options.parameters)).done(success).fail(failure);
        else $.post(options.url, SO.VM.unwrap(options.parameters)).done(success).fail(failure);

        //$ajax({
        //    url: options.url,
        //    data: ko.mapping.toJS(options.parameters),
        //    success: success,
        //    async: options.async,
        //    error: failure
        //});

    };

    /**
    *   Create an oservableArray for the specified model
    *   model: Model. The model of the collection.
    *   [urlOptions]: object { url: '', parameters: {}, useGet: true, async: true }. Note that url parameters can be observable
    *   [dataArray]: Array. Initial values
    *   [idProperty]: string. The name of the id property
    **/
    SO.ViewModel.observableCollection = function (model, urlOptions, dataArray, idProperty) {
        if (typeof model === 'undefined') throw new Error("Function observableCollection require at least 2 parameters");
        var vm = {};
        options = $.extend({}, { url: '', parameters: {}, useGet: true, async: true }, urlOptions);
        if(!options.url) throw new Error("URL must be specified");
 
        if (dataArray instanceof Array) {
            vm = ko.mapping.fromJS({ collection: dataArray }, {
                'collection': {
                    create: function (options) {
                        return new model(options.data);
                    }
                }
            });
        }
        else vm.collection = ko.observableArray();

        vm.collection.model = model;
        vm.collection.urlOptions = options;
        vm.collection.idPropertyName = idProperty;
        

        /**
        *   Fetch the collection
        *   [params]: object. Url parameters
        *   [overwrite]: boolean. Overwrite the parameters if true, merge if not
        *   [done]: function. Called after fetch succeded
        **/
        vm.collection.fetch = function (params, overwrite, done) {
        
            if (typeof overwrite === 'undefined') overwrite = false;
            if (typeof params !== 'undefined' && overwrite) vm.collection.urlOptions.parameters = params;
            else if (typeof params !== 'undefined') vm.collection.urlOptions.parameters = $.extend({}, vm.collection.urlOptions.parameters, params)
            var map = {};
            SO.ViewModel.request(vm.collection.urlOptions, function (data) {
            
                if (!(data instanceof Array)) data = [data];
                if (typeof idProperty === 'string') {
                    ko.mapping.fromJS({ collection: data }, {
                        'collection': {
                            key: function (data) {
                                return ko.utils.unwrapObservable(data[vm.collection.idPropertyName]);
                            },
                            create: function (options) {
                                return new model(options.data);
                            }
                        }
                    }, vm);
                } else {
                    ko.mapping.fromJS({ collection: data }, {
                        'collection': {
                            create: function (options) {
                                return new model(options.data);
                            }
                        }
                    }, vm);
                }
                done();
                //if (typeof map.collection !== 'undefined') collection(map.collection());
            });
        };

        return vm.collection;
    }

    /**
    *   Create the observable properties specified
    *   target: object. The object to populate
    *   poperties: object. The observables to create
    **/
    SO.mapping = function (target, properties, options) {
        if (typeof target === 'undefined') throw new Error("target must be defined");
        if (typeof properties === 'undefined') return target;
        options = $.extend({}, { models: {}, modelCollection: {} }, options);
        var type;
        var property;

        for (var key in properties) {
            property = properties[key];
            type = typeof property;

            if (property === null) continue;
            //Primitive
            if (type !== 'object' && type !== 'function') target[key] = ko.observable(property);

                //Object
            else if (type === 'object') {
                if (typeof options.models[key] !== 'undefined') target[key] = new options.models[key](property);
                else if (typeof options.modelCollection[key] !== 'undefined') {
                    target[key] = observableCollection(
                        options.modelCollection[key].model,
                        options.modelCollection[key].urlOptions);
                }
                else if (property instanceof Array) target[key] = ko.observableArray(property);
            }
                //Function Or Object
            else target[key] = property;
        }
        return target;
    }
    /**
    *   Return the JSON model of a ViewModel
    *   viewModel: ViewModel. The VM to unwrap
    **/
    SO.VM.unwrap = function (viewModel) {
        var model = (viewModel instanceof Array ? [] : {});
        var value;
        function ignore(key)
        {
            var ignorList = { '__ko_mapping__': '', 'prototype': '', 'constructor': '', '__proto__': '', 'settings': '', 'idPropertyName': '' };
            return typeof ignorList[key] !== 'undefined';
        }
        for (var key in viewModel) {
            if (ignore(key) ) continue;

            var v = viewModel[key];
            if (ko.isObservable(v) || (v instanceof ViewModel)) {
                value = ko.utils.unwrapObservable(v);
            }
            else if (typeof v === 'function') {
                continue;
            }
            else {
                value = v;
            }
            //Unrawp sub object recursivly
            if (typeof value === 'object') value = SO.VM.unwrap(ko.utils.unwrapObservable( value ));

            if (model instanceof Array) model.push(value);
            else model[key] = value;
        }
        return model;
    }

    SO.VM.onPropertyChanged = function ( vm,  propertyChanged ) {
        if (propertyChanged) {
            var subcriptions = [],
                subscription;
            $.each(vm, function (key, value) {
                if (ko.isObservable(vm[key])) {
                    subscription = vm[key].subscribe(function (value) {
                        propertyChanged(key, value);
                    });
                    subcriptions.push(subscription);
                }
            });
            return subcriptions;
        }
    };
    /**
    *   Bind a VM with an other. Only observable properties which match are updated
    *
    *   observer: VM. The listener.
    *   observed: VM. The target VM to observe.
    **/
    SO.VM.observe = function (observer, observed ) {
        var subcriptions = [];
        var subscription;

        $.each(observed, function (key, value) {
            if (ko.isObservable(observed[key]) && ko.isObservable(observer[key])) {
                if (observed[key]() !== observer[key]()) observer[key](observed[key]());    //Copy observed values in the observer VM if different.

                //Register on all observable properties
                subscription = observed[key].subscribe(function (value) {
                    if (!ko.isObservable(observer[key])) return;

                    observer[key](value);
                });
                subcriptions.push(subscription);
            }
        });
        observer._observes = function () { return subcriptions };   //Store the subcriptions in the VM observer
    };

    SO.VM.unobserveArray = function (array) {
        for (var idx in array) {
            array[idx].dispose();
        }
    };

    SO.VM.unobserve = function (observer) {
        if (typeof observer._observes === 'undefined') return;
        var subcriptions = observer._observes();
        SO.VM.unobserveArray(subcriptions);

        delete observer._observes;
    }

})();
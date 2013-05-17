(function (undefined) {
    var SO = window.SO;
    var Navigate = {
        toList: function () {
            $('#album-manager').carousel(0);
        },
        toCreate: function () {
            $('#album-manager').carousel(1);
        }
    }
    SO.VM.Artist = SO.ViewModel.extend({
        init: function (model) {
            
            this.Id = ko.observable(0);
            this.Name = ko.observable("Unknown");
            this.Biography = ko.observable("");
            this.Albums = ko.observableArray([]);
            this._super({}, model, 'Id');
        }
    });

    SO.VM.Album = SO.ViewModel.extend({
        init: function (model) {
            
            this.Id = ko.observable(0);
            this.Title = ko.observable("");
            this.Duration = ko.observable(0);
            this.ReleaseDate = ko.observable(new Date().getMonth() + '/' + new Date().getDate() + '/' + new Date().getFullYear());
            this.Type = ko.observable("");
            this.Artist = new SO.VM.Artist();
            this.Tracks = ko.observableArray([]);
            this._super({}, model, 'Id');
        },
        map: function (data, options) {
            this._super(data, options);
            if (this.Artist == null || this.Artist == undefined) this.Artist = new SO.VM.Artist();
        },
        create: function (callback) {
            var _this = this;
            $.ajax({
                url: SO.rootUrl + 'api/album',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(SO.VM.unwrap(_this)),
                success: function (data) {
                    if (callback != undefined) callback();
                    _this.clear();
                }
            });
        },
        update: function (callback) {
            var _this = this;
            $.ajax({
                url: SO.rootUrl + 'api/album',
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(SO.VM.unwrap(_this)),
                success: function (data) {
                    if (callback != undefined) callback();
                }
            });
        },
        remove: function () {
            $.ajax({
                url: SO.rootUrl + 'api/album',
                type: 'DELETE',
                contentType: 'application/json',
                data: JSON.stringify(SO.VM.unwrap(_this)),
                success: function (data) {
                    if (callback != undefined) callback();
                    _this.clear();
                }
            });
        }
    });

    SO.VM.AlbumCollection = SO.VM.Collection.extend({
        init: function () {
            this._super(SO.VM.Album, SO.rootUrl + 'api/album');
        },
        
    });
    SO.VM.Current.Album = {
        New: new SO.VM.Album(),
        create: function () {
            var _this = SO.VM.Current.Album;
            if (_this.New.Title().length == 0) return;
            Navigate.toCreate();
        },
        list: function(){
            Navigate.toList();
        },
        addAlbum: function (album) {
            var _this = SO.VM.Current.Album;
            album.create(function () {
                _this.Collection.fetch();
                _this.list();
            });
        },
        editAlbum: function (album) {
            var _this = SO.VM.Current.Album;
            SO.VM.Current.Album.Modal.Edit = new SO.VM.Album(album.model());
            ko.applyBindings(album, $('#modal-edit-album')[0]);
            $('#edit-save').off('click').on('click', function (e) {
                e.preventDefault();
                SO.VM.Current.Album.Modal.Edit.update(function () {
                    album.map(SO.VM.Current.Album.Modal.Edit.model());
                    $('#modal-edit-album').modal('hide');
                });
            });
            $('#modal-edit-album').modal('show');
        },
        deleteAlbum: function(album){
            $('#modal-delete-album').modal('show');
        },
        Collection: new SO.VM.AlbumCollection(),
        Modal: {
            Edit: new SO.VM.Album()
        }
    }
    $(function () {
        $('.carousel').carousel({
            interval: 0
        })
    });
})();
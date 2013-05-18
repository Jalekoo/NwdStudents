(function (undefined) {
    SO.VM.Artist = SO.ViewModel.extend({
        init: function (model) {

            this.Id = ko.observable(0);
            this.Name = ko.observable("Unknown");
            this.Biography = ko.observable("");
            this.Albums = ko.observableArray([]);
            this._super({}, model, 'Id');
        },
        create: function (callback) {
            var _this = this;
            $.ajax({
                url: SO.rootUrl + 'api/artist',
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
                url: SO.rootUrl + 'api/artist',
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
                url: SO.rootUrl + 'api/artist',
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
    SO.VM.ArtistCollection = SO.VM.Collection.extend({
        init: function () {
            this._super(SO.VM.Artist, SO.rootUrl + 'api/artist');
        },
    });
    SO.VM.Album = SO.ViewModel.extend({
        init: function (model) {

            this.Id = ko.observable(0);
            this.Title = ko.observable("");
            this.Duration = ko.observable(0);
            this.ReleaseDate = ko.observable(new Date().getMonth() + '/' + new Date().getDate() + '/' + new Date().getFullYear());
            this.Type = ko.observable("");
            this.Artist = new SO.VM.Artist();
            this.CoverImagePath = ko.observable("");
            this._super({}, model, 'Id');
            this.Tracks = {};
            
            //this.Tracks.fetch();
        },
        //override
        map: function (data, options) {
            this._super(data,options);
            if (this.Artist == null || this.Artist == undefined) this.Artist = new SO.VM.Artist();

        },
        display: function(){

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
    SO.VM.Song = SO.ViewModel.extend({
        init: function (model) {
            this.Id         = ko.observable(),
            this.Name       = ko.observable(),
            this.Composed   = ko.observable(),
            this.Features   = ko.observable()
        }
    });
    SO.VM.Track = SO.ViewModel.extend({
        init: function (model) {
            this.AlbumId = ko.observable(0);
            this.Number = ko.observable(0);
            this.Duration = ko.observable(0);
            this.Song = new SO.VM.Song();
            this.Album = new SO.VM.Album();
        }
    });
    SO.VM.TrackCollection = SO.VM.Collection.extend({
        init: function (albumId) {
            this._super(SO.VM.Track, SO.rootUrl + 'api/track');
        },
    });
})();
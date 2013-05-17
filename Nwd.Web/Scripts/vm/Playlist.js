(function (undefined) {
    SO.VM.Playlist = SO.ViewModel.extend({
        init: function (model) {
            this.Id = ko.observable(0);
            this.Title = ko.observable('');
            this.Tracks = {};
            this._super({}, model);
        },
        map: function (data) {
            this._super(data, {
                Tracks :{ 
                    create: function (o) {
                        this.Tracks = new SO.VM.PlaylistTrackCollection(data.Id);
                    }
                }
            });
        },
        create: function (callback) {
            var _this = this;
            $.ajax({
                url: SO.rootUrl + 'api/playlist',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ playlistName: this.Title() }),
                success: function (data) {
                    if (typeof callback !== 'undefined') callback();
                    _this.clear();
                }
            });
        },
        remove: function (callback) {
            var _this = this;
            $.ajax({
                url: SO.rootUrl + 'api/playlist',
                type: 'DELETE',
                contentType: 'application/json',
                data: JSON.stringify({ playlistName : this.Title() }),
                success: function (data) {
                    if (callback != undefined) callback();
                    _this.clear();
                }
            });
        }
    });
    SO.VM.PlaylistCollection = SO.VM.Collection.extend({
        init: function () {
            this._super(SO.VM.Playlist, SO.rootUrl + 'api/playlist');
        },
    });
    SO.VM.PlaylistTrack = SO.ViewModel.extend({
        init: function (model) {
            this.SongName = ko.observable('');
            this.SongUrl = ko.observable('');

            this._super({}, model);
        }
    });
    SO.VM.PlaylistTrackCollection = SO.VM.Collection.extend({
        init: function (id) {
            this._super(SO.VM.PlaylistTrack, SO.rootUrl + 'api/playlistTrack/?Id='+id);
        },
    });
})();
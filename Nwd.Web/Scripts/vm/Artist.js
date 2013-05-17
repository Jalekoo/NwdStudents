(function (undefined) {
    var Navigate = {
        toList: function () {
            $('#artist-manager').carousel(0);
        },
        toCreate: function () {
            $('#artist-manager').carousel(1);
        }
    }

    SO.VM.Current.Artist = {
        New: new SO.VM.Artist(),
        create: function () {
            var _this = SO.VM.Current.Artist;
            if (_this.New.Name().length == 0) return;
            Navigate.toCreate();
        },
        list: function () {
            Navigate.toList();
        },
        addArtist: function (artist) {
            var _this = SO.VM.Current.Artist;
            artist.create(function () {
                _this.Collection.fetch();
                _this.list();
            });
        },
        editArtist: function (artist) {
            var _this = SO.VM.Current.Artist;
            SO.VM.Current.Artist.Modal.Edit = new SO.VM.Artist(artist.model());
            ko.applyBindings(artist, $('#modal-edit-artist')[0]);
            $('#edit-save').off('click').on('click', function (e) {
                e.preventDefault();
                SO.VM.Current.Artist.Modal.Edit.update(function () {
                    artist.map(SO.VM.Current.Artist.Modal.Edit.model());
                    $('#modal-edit-artist').modal('hide');
                });
            });
            $('#modal-edit-artist').modal('show');
        },
        deleteArtist: function (artist) {
            $('#modal-delete-artist').modal('show');
        },
        Collection: new SO.VM.ArtistCollection(),
        Modal: {
            Edit: new SO.VM.Artist()
        }
    }
    $(function () {
        $('.carousel').carousel({
            interval: 0
        })
    });
})();
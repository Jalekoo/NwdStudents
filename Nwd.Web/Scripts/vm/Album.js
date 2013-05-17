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
    
    SO.VM.Current.Album = {
        New: new SO.VM.Album(),
        View: new SO.VM.Album(),
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
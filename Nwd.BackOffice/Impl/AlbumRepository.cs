﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Nwd.BackOffice.Model;

namespace Nwd.BackOffice.Impl
{
    public class AlbumRepository
    {
        FileRepository _fileRepo;

        public AlbumRepository()
        {
            _fileRepo = new FileRepository();
        }
        
        public List<Album> GetAllAlbums()
        {
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Albums.ToList();
            }
        }

        public bool AlbumExists( Album album )
        {
            if( album == null )
            {
                throw new ArgumentNullException( "album" );
            }
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Albums.Any( a => a.Title == album.Title );
            }
        }

        public Album CreateAlbum( Album album, HttpServerUtilityBase server )
        {
                if( album == null )
            {
                throw new ArgumentNullException( "album" );
            }
            if( server == null )
            {
                throw new ArgumentNullException( "server" );
            }

            using( var ctx = new NwdBackOfficeContext() )
            {
                album = ctx.Albums.Add( album );

                string directory;
                string physDirectory;
                EnsureDirectory( album, server, out directory, out physDirectory );
                if( album.Tracks != null )
                {
                    foreach( var track in album.Tracks )
                    {
                        HttpPostedFileBase file = track.File;
                        if( file == null ) throw new ApplicationException( "You must add a file to a track" );

                        string fileName = SaveFile( physDirectory, file );
                        track.FileRelativePath = Path.Combine( directory, fileName );
                    }
                }

                if( album.CoverImagePath == null || album.CoverImagePath.Length == 0 )
                    album.CoverImagePath = "album.jpg";
                
                string coverFileName = "cover.jpg";
                string physPath = Path.Combine( physDirectory, coverFileName );
                _fileRepo.SaveFile( album.CoverImagePath, physPath );

                album.CoverImagePath = Path.Combine( directory, coverFileName );
                

                ctx.SaveChanges();  
                return album;
            }
        }

        public List<Track> GetTrack( int idAlbum )
        {
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Albums.Where( a => a.Id == idAlbum ).SelectMany( m => m.Tracks ).ToList();
            }
        }

        public Album GetAlbumForEdit( int idAlbum )
        {
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Albums.Include( "Tracks" ).Include( "Tracks.Song" ).Include( "Artist" ).SingleOrDefault( a => a.Id == idAlbum );
            }
        }

        public Album EditAlbum( Album album, HttpServerUtilityBase server )
        {

            using( var ctx = new NwdBackOfficeContext() )
            {
                album = ctx.Albums.Attach( album );
                ctx.Entry( album ).Reference( e => e.Artist ).Load();
                ctx.Entry( album ).Collection( e => e.Tracks ).Load();
                
                string directory;
                string physDirectory;
                EnsureDirectory( album, server, out directory, out physDirectory );

                foreach( var track in album.Tracks )
                {
                    HttpPostedFileBase file = track.File;
                    if( file != null )
                    {
                        //TODO delete previous file
                        string fileName = SaveFile( physDirectory, file );
                        track.FileRelativePath = Path.Combine( directory, fileName );
                    }

                    //else do not change the FileRelativePath since it is send by the form in an hidden input
                }
                if( GetAlbumForEdit( (int)album.Id ).CoverImagePath != album.CoverImagePath )
                {
                    if( album.CoverImagePath == null || album.CoverImagePath.Length == 0 )
                        album.CoverImagePath = "album.jpg";

                    string coverFileName = "cover.jpg";
                    string physPath = Path.Combine( physDirectory, coverFileName );
                    _fileRepo.SaveFile( album.CoverImagePath, physPath );

                    album.CoverImagePath = Path.Combine( directory, coverFileName );
                }

                ctx.Entry( album ).State = System.Data.EntityState.Modified;
                //foreach( var e in ctx.ChangeTracker.Entries() )
                //{
                //    e.State = System.Data.EntityState.Modified;
                //}
                ctx.SaveChanges();
                return album;
            }
        }

        private static void EnsureDirectory( Album album, HttpServerUtilityBase server, out string directory, out string physDirectory )
        {
            directory = String.Format( "/Content/musics/{0}/", album.Title );
            physDirectory = server.MapPath( directory );
            if( !Directory.Exists( physDirectory ) )
            {  
                Directory.CreateDirectory( physDirectory );
            }
        }

        private static string SaveFile( string physDirectory, HttpPostedFileBase file )
        {
            if( Path.GetExtension( file.FileName ) != ".mp3" ) throw new ApplicationException( "The file must be an .mp3 file" );

            string fileName = Path.GetFileName( file.FileName );
            string physPath = Path.Combine( physDirectory, fileName );
            file.SaveAs( physPath );
            return fileName;
        }
    }
}

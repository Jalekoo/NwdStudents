using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nwd.BackOffice.Model;

namespace Nwd.Web.Areas.Api.Models
{
    public class TrackRegisterViewModel
    {
        public int AlbumId { get; set; }

        public long Duration { get; set; }

        public Song Song { get; set; }
    }
}

// Artist.cs
//
//  Copyright (C) 2008 Amr Hassan
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using System.Collections.Generic;
using System.Xml;

namespace lastfm.Services
{
	public class Artist : TaggableBase
	{
		public string Name {get; private set;}
    
    public ArtistBio Bio
    {
      get
      { return new ArtistBio(Name, getAuthData()); }
    }
    
		public Artist(string name, string apiKey, string secret, string sessionKey)
      :base("artist", new string[] {apiKey, secret, sessionKey})
		{
      this.Name = name;
		}
    
    public Artist(string name, string[] authData)
      :base(name, authData)
    {
      Name = name;
    }
    
    public override string ToString ()
    {
      return this.Name;
    }
    
    protected override RequestParameters getParams ()
    {
      RequestParameters p = base.getParams();
      p["artist"] = this.Name;
      
      return p;
    }
    
    public Artist[] GetSimilar(int limit)
    {
      RequestParameters p = getParams();
      if (limit > -1)
        p["limit"] = limit.ToString();
      
      XmlDocument doc = request("artist.getSimilar", p);
      
      string[] names = extractAll(doc.DocumentElement, "name");
      
      List<Artist> list = new List<Artist>();
      
      foreach(string name in names)
        list.Add(new Artist(name, getAuthData()));
      
      return list.ToArray();
    }

    public Artist[] GetSimilar()
    {
      return GetSimilar(-1);
    }
    
    public int GetListenerCount()
    {
      XmlDocument doc = request("artist.getInfo");
      
      return Convert.ToInt32(extract(doc.DocumentElement, "listeners"));
    }
    
    
	}
}

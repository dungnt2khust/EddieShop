using Imagekit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EddieShop.Core.Utilities
{
    public class UploadImage
    {
        public UploadImage()
        {

        }
        [Obsolete]
        public AuthParamResponse Upload()
        {
            Imagekit.Imagekit imagekit = new Imagekit.Imagekit(
                "public_Rk9tSR2EEq/ahhSc5lLKIUrIYTM=",
                "private_fO8TZSBl52IV3D2I7CphZILcrHg=",
                "https://ik.imagekit.io/uvn3cxjawn6"
            );
            AuthParamResponse resp = imagekit.GetAuthenticationParameters();
            return resp;
        }
    }
}

﻿using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace NCombo
{
    public class ComboHandler : BaseHttpHandler
    {
        // TODO: make this configurable
        private string yuiDir = "~/yui/";

        public override void HandleRequest(HttpContextBase context)
        {

            string q = context.Request.Url.Query.Substring(1);

            var paths =
                from path in q.Split('&')
                where !string.IsNullOrEmpty(path)
                select VirtualPathUtility.ToAbsolute(yuiDir + path, context.Request.ApplicationPath);

            //
            // Set Mime Type
            //
            if ( isCSS(paths.First()) ) {
                context.Response.ContentType = "text/css";
            }
            else {
                context.Response.ContentType = "application/x-javascript";
            }

            // TODO: cache
            // copy individual file streams to the output
            foreach (string p in paths) {
                string filePath = context.Server.MapPath(p);
                if (!File.Exists(filePath)) {
                    RespondFileNotFound(context);
                }
                string contents = File.ReadAllText(filePath);

                if (isCSS(p)) {
                    contents = fixupCss(p, contents);
                }

                context.Response.Write(contents);
                context.Response.Write('\n');
            }
        }

        /// <summary>
        /// When combo-loading, css paths get mixed up. Must fix that
        /// </summary>
        /// <remarks>
        /// Regular Expressions and logic borrowed from the PHP Loader:
        /// https://github.com/yui/phploader/blob/master/phploader/combo.php
        /// The PHP Loader is Copyright (c) 2009, Yahoo! Inc. All rights reserved.
        /// Code licensed under the BSD License:
        /// http://developer.yahoo.net/yui/license.html
        /// version: 1.0.0b2
        /// </remarks>
        private string fixupCss(string path, string contents)
        {
            //string basePath = 

            return contents;
            /*
                css post-processing from the php loader:
                $cssResourceList = $loader->css_data();
            foreach ($cssResourceList["css"] as $cssResource=>$val) {
                foreach (
                    $cssResourceList["css"][$cssResource] as $key=>$value
                ) {
                     $crtResourceBase = substr($key, 0, strrpos($key, "/") + 1);
                     $crtResourceContent = $loader->getRemoteContent($key);
                     
                     //Handle image path corrections (order is important)
                     $crtResourceContent = preg_replace(
                         '/((url\()([^\.\.|^http]\S+)(\)))/', '${2}'. 
                         $crtResourceBase . '${3}${4}', $crtResourceContent
                     ); // just filename or subdirs/filename (e.g) url(foo.png),
                        // url(foo/foo.png)
                     $crtResourceContent = str_replace(
                         "url(/", 
                         "url($crtResourceBase", $crtResourceContent
                     ); // slash filename (e.g.) url(/whatever)
                     $crtResourceContent = preg_replace(
                         '/(url\()(\.\.\/)+/', 
                         'url(' . $base, $crtResourceContent
                     ); // relative paths (e.g.) url(../../foo.png)
                     $crtResourceContent = preg_replace_callback(
                         '/AlphaImageLoader\(src=[\'"](.*?)[\'"]/',
                         'alphaImageLoaderPathCorrection',
                         $crtResourceContent
                     ); // AlphaImageLoader relative paths (e.g.) 
                        // AlphaImageLoader(src='../../foo.png')
                     
                     $rawCss .= $crtResourceContent;
                }
            }
            
            //Cleanup build path dups caused by relative paths that already
            //included the build directory
            $rawCss = str_replace("/build/build/", "/build/", $rawCss);

             */
        }

        private bool isCSS(string path)
        {
            return Path.GetExtension(path) == ".css";
        }

        public override bool ValidateParameters(HttpContextBase context)
        {
            // TODO: validation on query string
            return true;
        }

        public override void SetResponseCachePolicy(HttpCachePolicyBase cache)
        {
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.AddYears(10));
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }
    }
}
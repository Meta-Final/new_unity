using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReqRes
{
    [Serializable]
    public class AuthURL
    {
        public string authURL = "http://metaai2.iptime.org:14596/auth";
    }
    
    [Serializable]
    public class ArticleURL
    {
        public string searchURL = "http://metaai2.iptime.org:14596/search";
        public string createURL = "http://metaai2.iptime.org:14596/create";
        public string deleteURL = "http://metaai2.iptime.org:14596/delete";
        public string updateURL = "http://metaai2.iptime.org:14596/update";
        public string getURL = "http://metaai2.iptime.org:14596/get";

    }

    [Serializable]
    public class AIURL
    {
        public string chatURL = "http://metaai2.iptime.org:14596/chat";
        public string genCoverURL = "http://metaai2.iptime.org:14596/gencover";
        public string genObjectURL = "http://metaai2.iptime.org:14596/genobject";
        public string loadCoverURL = "http://metaai2.iptime.org:14596/loadcover";
        public string loadObjectURL = "http://metaai2.iptime.org:14596/loadobject";
        
    }

    //Search request/response
    [Serializable]
    public class SearchRequest
    {
        public string query;
        public int limit;
    }

    [Serializable]
    public class SearchResponse
    {
        public List<SearchResult> request;
    }

    [Serializable]
    public class SearchResult
    {
        public string id;
        public string title;
        public string snippet;
        public float score;
    }

    //Auth request
    [Serializable]
    public class AuthRequest
    {
        public string userid;
    }
    //Create/Get request/response
    [Serializable]
    public class Article
    {   
        public string userid;
        public string postId;
        public List<Elements> elements;
    }

    [Serializable]
    public class Elements
    {
        public string content;
        public int type;
        public string imageData;
        public Position position;
        public Scale scale;
        public int fontSize;
        public string fontFace;
        public bool isUnderlined;
        public bool isStrikethrough;
    }

    [Serializable]
    public class Position 
    {
        public float x;
        public float y;
        public float z;

    }

    [Serializable]
    public class Scale
    {
        public float x;
        public float y;
        public float z;
    }

    //Delete request/response
    [Serializable]
    public class DeleteRequest
    {
        public string text;
    }

    //LLM chat request/response
    //Gen image request/response
    [Serializable]
    public class GenCoverRequest
    {
        public string text;
    }

    [Serializable]
    public class GenCoverResponse
    {
        public string imgPath;
    }
    //Load image request/response
    [Serializable]
    public class LoadCoverRequest
    {
        public string imgPath;
    }

    [Serializable]
    public class LoadCoverResponse
    {
        public Article article;
        public string coverImg;
    }

    //Gen object request/response
    [Serializable]
    public class GenObjectRequest
    {
        public string text;
    }
    [Serializable]
    public class GenObjectResponse
    {
        public string objFilePath;
        public string mtlFilePath;
    }
    //Load object request/response
    [Serializable]
    public class LoadObjectRequest
    {
        public string objFilePath;
        public string mtlFilePath;
    }
    [Serializable]
    public class LoadObjectResponse
    {
        public string objBinary;
        public string mtlBinary;
    }
}

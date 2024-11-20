using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReqRes
{
    [Serializable]
    public class AuthURL
    {
        public string authURL = "http://metaai2.iptime.org:14596/auth/create-user";
    }
    
    [Serializable]
    public class ArticleURL
    {
        public string searchURL = "http://metaai2.iptime.org:14596/articles/search";
        public string createURL = "http://metaai2.iptime.org:14596/articles/create";
        public string getURL = "http://metaai2.iptime.org:14596/articles/get";
        public string deleteURL = "http://metaai2.iptime.org:14596/articles/delete";
    }

    [Serializable]
    public class AIURL
    {
        public string chatURL = "http://metaai2.iptime.org:14596/chat/message";
        public string loadCoverURL = "http://metaai2.iptime.org:14596/loadcover";
        public string loadObjectURL = "http://metaai2.iptime.org:14596/loadobject";
        public string trendURL = "http://metaai2.iptime.org:14596/";
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
        public string userId;
        public string name;
        public string nickName;
    }
    //Create/Get request/response
    [Serializable]
    public class Article
    {   
        public AuthRequest authRequest;
        public string articleId;
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
        public string userId;
        public string articleId;
        public string text;
    }

    //LLM chat request/response
    [Serializable]
    public class ChatRequest
    {
        public string userId;
        public string text;
    }

    [Serializable]
    public class ChatResponse
    {
        public string text;
        public Files files;
    }

    [Serializable]
    public class Files
    {
        public string imgPath;
        public string objPath;
    }

    //Load image request
    [Serializable]
    public class LoadCoverRequest
    {
       public List<Files> filePath;
    }

    //Load object request
    [Serializable]
    public class LoadObjectRequest
    {
        public List<Files> filePath;
    }


    //알파때 URL
    [Serializable]
    public class AlphaURL
    {
        public string coverURL = "http://metaai2.iptime.org:14596/thumbnail";
        public string trendURL = "http://metaai2.iptime.org:14596/trend";
    }
}

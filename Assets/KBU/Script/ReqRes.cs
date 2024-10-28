using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReqRes
{
    [Serializable]
    public class URL
    {
        public string chatUrl = "http://metaai2.iptime.org:14596/chat";
        public string trendUrl = "http://metaai2.iptime.org:14596/trend";

    }

    [Serializable]
    public class ChatRequest
    {
        public string text;
    }

    [Serializable]
    public class ChatResponse
    {
        public string text;
    }

    [Serializable]
    public class TrendRequest
    {
        public string text;
    }

    [Serializable]
    public class TrendResponse
    {
        public string[] trend;
    }
}

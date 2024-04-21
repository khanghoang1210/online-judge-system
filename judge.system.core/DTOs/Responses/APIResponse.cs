﻿namespace judge.system.core.DTOs.Responses
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

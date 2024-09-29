namespace Master.Core.Wrappers
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }

        public BaseResponse(T data)
        {
            IsSuccess = true;
            StatusCode = (int)StatusCodes.OK;
            Data = data;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }
        public int StatusCode { get; set; }

        public static BaseResponse<T> Success(T data) => new BaseResponse<T> { IsSuccess = true, StatusCode = (int)StatusCodes.OK, Data = data };
        public static BaseResponse<T> Success(string message) => new BaseResponse<T> { IsSuccess = true, StatusCode = (int)StatusCodes.OK, Message = message };
        public static BaseResponse<T> Success(T data, string message) => new BaseResponse<T> { IsSuccess = true, StatusCode = (int)StatusCodes.OK, Data = data, Message = message };
        public static BaseResponse<T> NotFound(string message) => new BaseResponse<T> { IsSuccess = true, StatusCode = (int)StatusCodes.NoContent, Message = message };
        public static BaseResponse<T> NotFound(string message, int statusCode) => new BaseResponse<T> { IsSuccess = true, StatusCode = statusCode, Message = message };
        public static BaseResponse<T> NotFound(string message, int statusCode, bool isSuccess) => new BaseResponse<T> { IsSuccess = isSuccess, StatusCode = statusCode, Message = message };
        public static BaseResponse<T> Unauthorized(string message) => new BaseResponse<T> { IsSuccess = true, StatusCode = (int)StatusCodes.Unauthorized, Message = message };
        public static BaseResponse<T> BadRequest(string error) => new BaseResponse<T> { IsSuccess = false, StatusCode = (int)StatusCodes.BadRequest, Errors = new() { error } };
        public static BaseResponse<T> BadRequest(T data, string error) => new BaseResponse<T> { IsSuccess = false, StatusCode = (int)StatusCodes.BadRequest, Errors = new() { error }, Data = data };
        public static BaseResponse<T> BadRequest(T data, string error, int statusCode) => new BaseResponse<T> { IsSuccess = false, StatusCode = statusCode, Errors = new() { error }, Data = data };
        public static BaseResponse<T> BadRequest(string message, int statusCode) => new BaseResponse<T> { IsSuccess = false, StatusCode = statusCode, Message = message };
        public static BaseResponse<T> BadRequest(List<string> errors, int statusCode) => new BaseResponse<T> { IsSuccess = false, StatusCode = statusCode, Errors = errors };
        public static BaseResponse<T> BadRequest(List<string> errors, string message, int statusCode) => new BaseResponse<T> { IsSuccess = false, StatusCode = statusCode, Errors = errors, Message = message };

    }

    public enum StatusCodes
    {
        OK = 200,
        NotFound = 404,
        BadRequest = 400,
        NoContent = 204,
        Unauthorized = 401
    }

    public enum HttpStatusCode
    {
        //1xx - Informational
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,
        EarlyHints = 103,

        // 2xx Success
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        AlreadyReported = 208,
        IMUsed = 226,

        // 3xx Redirection
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        // 4xx Client Error
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        PayloadTooLarge = 413,
        URITooLong = 414,
        UnsupportedMediaType = 415,
        RangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        ImATeapot = 418,
        MisdirectedRequest = 421,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        TooEarly = 425,
        UpgradeRequired = 426,
        PreconditionRequired = 428,
        TooManyRequests = 429,
        RequestHeaderFieldsTooLarge = 431,
        UnavailableForLegalReasons = 451,

        // 5xx Server Error
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HTTPVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,
        NotExtended = 510,
        NetworkAuthenticationRequired = 511

    }
}

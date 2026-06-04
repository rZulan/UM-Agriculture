using System.Net;

namespace Application.Results
{
    public class PaginationInfo
    {
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }

    public class SortInfo
    {
        public required List<string> SortColumns { get; set; }
        public List<string> SortDirections { get; set; } = ["asc", "dsc"];
        public CurrentSort? CurrentSort { get; set; }
    }

    public class CurrentSort
    {
        public string? Column { get; set; }
        public string? Direction { get; set; }
    }

    public class Result<T>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public HttpStatusCode? StatusCode { get; init; }
        public string? Message { get; init; }
        public T? Value { get; init; }


        protected Result(bool isSuccess, T? value, string? message, HttpStatusCode? statusCode)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Value = value;
            Message = message;
        }

        public static Result<T> Success(T? value, string? message = null, HttpStatusCode? statusCode = HttpStatusCode.OK)
            => new(true, value, message ?? "Operation successful.", statusCode);

        public static Result<T> Failure(string? message = null, HttpStatusCode? statusCode = HttpStatusCode.BadRequest)
            => new(false, default, message ?? "Operation failed.", statusCode);

        public static Result<T> FailureWithValue(T? value, string? message = null, HttpStatusCode? statusCode = HttpStatusCode.BadRequest)
            => new(false, value, message ?? "Operation failed.", statusCode);
    }

    public class GetAllResult<T>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public HttpStatusCode? StatusCode { get; init; }
        public string? Message { get; init; }
        public T? Value { get; init; }
        public PaginationInfo? Pagination { get; init; }
        public SortInfo? Sorting { get; set; }

        protected GetAllResult(bool isSuccess, T? value, string? message, HttpStatusCode? statusCode, PaginationInfo? paginationInfo = null, SortInfo? sorting = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Value = value;
            Message = message;
            Pagination = paginationInfo;
            Sorting = sorting;
        }

        public static GetAllResult<T> Success(T? value, string? message = null, HttpStatusCode? statusCode = HttpStatusCode.OK, PaginationInfo? paginationInfo = null, SortInfo? sortInfo = null)
            => new(true, value, message ?? "Operation successful.", statusCode, paginationInfo, sortInfo);

        public static GetAllResult<T> Failure(string? message = null, HttpStatusCode? statusCode = HttpStatusCode.BadRequest, PaginationInfo? paginationInfo = null, SortInfo? sortInfo = null)
            => new(false, default, message ?? "Operation failed.", statusCode, paginationInfo, sortInfo);
        public static GetAllResult<T> FailureWithValue(T? value, string? message = null, HttpStatusCode? statusCode = HttpStatusCode.BadRequest, PaginationInfo? paginationInfo = null, SortInfo? sortInfo = null)
            => new(false, value, message ?? "Operation failed.", statusCode, paginationInfo, sortInfo);
    }
}
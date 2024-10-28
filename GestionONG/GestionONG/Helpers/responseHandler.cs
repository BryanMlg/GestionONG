public static class responseHandler
{
    public static ApiResponse<T> Success<T>(T? data, string message = "Proceso Realizado con Exito")
    {
        return new ApiResponse<T> { success = true, data = data, message = message };
    }

    public static ApiResponse<T> Error<T>(string message)
    {
        return new ApiResponse<T> { success = false, message = message };
    }
}

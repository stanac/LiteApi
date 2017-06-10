namespace LiteApi
{
    /// <summary>
    /// JSON action result that can return any JSON response
    /// </summary>
    /// <seealso cref="LiteApi.ContentActionResult" />
    public class JsonActionResult : ContentActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonActionResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public JsonActionResult(string data) : base(data, "application/json")
        {
        }
    }
}

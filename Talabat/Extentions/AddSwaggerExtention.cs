namespace Talabat.Extentions
{
    public static class AddSwaggerExtention
    {
        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}


using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;



namespace CatfishExtensions
{
    public static class CatfishExtensions
    {
        public static WebApplicationBuilder AddCatfishExtensions(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            CorsHelper.AddPolicies(configuration, services);

            services.AddSingleton<ICatfishWebClient, CatfishWebClient>();
            services.AddScoped<IJwtProcessor, JwtProcessor>();
            services.AddScoped<IGoogleIdentity, GoogleIdentity > ();

            //MR Janu 26 2023
           // builder = AddCatfishJwtAuthorization(builder, true);

            return builder;
        }

        /// <summary>
        /// Uses catfish extensions in the web application
        /// </summary>
        /// <param name="application">The current web application</param>
        /// <returns>The web application</returns>
        public static WebApplication UseCatfishExtensions(this WebApplication app)
        {
            app.UseCors();

            //MR Jan 26 2023
         //   app =UseCatfishJwtAuthorization(app, true);

            return app;
        }

        public static WebApplicationBuilder AddCatfishJwtAuthorization(this WebApplicationBuilder builder, bool configureSwagger)
        {
            // Adding JWT Bearer authorization
            if (configureSwagger)
            {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
            }

            //JWT Extension

            ConfigurationManager configuration = builder!.Configuration;
            //_configuration.GetSection("Google:Identity:rsa_privateKey").Value;
            string jwtValidAudience = configuration.GetSection("JwtConfig:Audience").Value;
            string jwtValidIssuer = configuration.GetSection("JwtConfig:Issuer").Value;

            string jwtPublicKey = configuration.GetSection("JwtConfig:RsaPublicKey").Value;//File.ReadAllText(configuration["JwtConfig:RsaPublicKey"]) ;
            if (jwtPublicKey.IndexOf("public key", StringComparison.OrdinalIgnoreCase) > 0)
                jwtPublicKey = jwtPublicKey.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];

            //Configure validation of JWT signed with a private asymetric key. We will use a public key to validate if the token was signed
            //with the corresponding private key.
            builder.Services.AddSingleton<RsaSecurityKey>(provider =>
            {
                // It's required to register the RSA key with depedency injection.
                // If you don't do this, the RSA instance will be prematurely disposed.
                RSA rsa = RSA.Create();

                rsa.ImportRSAPublicKey(
                    source: Convert.FromBase64String(jwtPublicKey),
                    bytesRead: out int _);

                return new RsaSecurityKey(rsa);
            });

            //Adding Authentication with JWT Bearer
            builder.Services.AddAuthentication()
                .AddJwtBearer("Asymmetric", options =>
                {
                    SecurityKey rsa = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

                    options.IncludeErrorDetails = true; // <- great for debugging

        // Configure the actual Bearer validation
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = rsa,
                        ValidAudience = jwtValidAudience,
                        ValidIssuer = jwtValidIssuer,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
                        ValidateLifetime = true, // <- the "exp" will be validated
                        ValidateAudience = true,
                        ValidateIssuer = true,
                    };
                });

            //Adding default authentication schema to be Asymmetric
            builder.Services.AddAuthentication("Asymmetric");

            return builder;
        }
        public static WebApplication UseCatfishJwtAuthorization(this WebApplication app, bool useSwagger)
        {
            if (useSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

    }
}

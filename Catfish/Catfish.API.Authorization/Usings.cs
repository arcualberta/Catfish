//System namespaces
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Runtime.Serialization;
global using Microsoft.AspNetCore.Cors;
global using System.IdentityModel.Tokens.Jwt;
global using Microsoft.IdentityModel.Tokens;
global using System.Text.Json;
global using System.Text.Json.Serialization;


//Shared namespaces
global using CatfishExtensions;
global using CatfishExtensions.Constants;
global using CatfishExtensions.Helpers;
global using CatfishExtensions.Interfaces;
global using CatfishExtensions.Exceptions;

//Project namespaces
global using Catfish.API.Authorization.Interfaces;
global using Catfish.API.Authorization.Services;
global using Catfish.API.Authorization.Models;
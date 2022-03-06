// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [Authorize]
    public class DiagnosticsController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public DiagnosticsController(IWebHostEnvironment environment, ILogger<DiagnosticsController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Environment: {_environment.EnvironmentName}");

            if (_environment.IsDevelopment())
            {
                var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
                return View(model);
            }

            return NotFound();
        }
    }
}
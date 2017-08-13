﻿// <copyright file="ApdexMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.AspNetCore.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.TrackingMiddleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ApdexMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string ApdexItemsKey = "__App.Metrics.Apdex__";
        private readonly RequestDelegate _next;
        private readonly ILogger<ApdexMiddleware> _logger;
        private readonly IApdex _apdexTracking;

        public ApdexMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILogger<ApdexMiddleware> logger,
            IMetrics metrics)
        {
            _next = next;
            _logger = logger;
            _apdexTracking = metrics.Provider
                                    .Apdex
                                    .Instance(HttpRequestMetricsRegistry.ApdexScores.Apdex(metricsAspNetCoreOptionsAccessor.Value.ApdexTSeconds));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting(GetType());

            context.Items[ApdexItemsKey] = _apdexTracking.NewContext();

            await _next(context);

            var apdex = context.Items[ApdexItemsKey];

            using (apdex as IDisposable)
            {
            }

            context.Items.Remove(ApdexItemsKey);

            _logger.MiddlewareExecuted(GetType());
        }
    }
}
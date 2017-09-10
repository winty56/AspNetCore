﻿// <copyright file="MetricsEndpointsOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Endpoints.Internal
{
    /// <summary>
    ///     Sets up default metric endpoint options for <see cref="MetricEndpointsOptions"/>.
    /// </summary>
    public class MetricsEndpointsOptionsSetup : IConfigureOptions<MetricEndpointsOptions>
    {
        private readonly EnvFormatterCollection _envFormatters;
        private readonly MetricsFormatterCollection _metricsFormatters;

        public MetricsEndpointsOptionsSetup(EnvFormatterCollection envFormatters, MetricsFormatterCollection metricsFormatters)
        {
            _envFormatters = envFormatters;
            _metricsFormatters = metricsFormatters;
        }

        /// <inheritdoc />
        public void Configure(MetricEndpointsOptions options)
        {
            if (options.MetricsTextEndpointOutputFormatter == null)
            {
                options.MetricsTextEndpointOutputFormatter =
                    _metricsFormatters.GetType<MetricsTextOutputFormatter>() ?? _metricsFormatters.LastOrDefault();
            }

            if (options.MetricsEndpointOutputFormatter == null)
            {
                options.MetricsEndpointOutputFormatter =
                    _metricsFormatters.GetType<MetricsJsonOutputFormatter>() ?? _metricsFormatters.LastOrDefault();
            }

            if (options.EnvInfoEndpointOutputFormatter == null)
            {
                options.EnvInfoEndpointOutputFormatter =
                    _envFormatters.GetType<EnvInfoTextOutputFormatter>() ?? _envFormatters.LastOrDefault();
            }
        }
    }
}
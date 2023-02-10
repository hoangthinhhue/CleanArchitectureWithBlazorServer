﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Core.Events;

    public class CustomerCreatedEvent : DomainEvent
    {
        public CustomerCreatedEvent(Customer item)
        {
            Item = item;
        }

        public Customer Item { get; }
    }


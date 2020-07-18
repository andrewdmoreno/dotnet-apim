﻿using Newtonsoft.Json;

namespace Apim.DevOps.Toolkit.ApimEntities.Subscription
{
	public class SubscriptionProperties
	{
		/// <summary>
		/// User (user id path) for whom subscription is being created in form /users/{userId}
		/// </summary>
		public string OwnerId { get; set; }

		/// <summary>
		/// Scope like /products/{productId} or /apis or /apis/{apiId}.
		/// </summary>
		[JsonProperty(Required = Required.Always)]
		public string Scope { get; set; }


		/// <summary>
		/// Subscription name.
		/// </summary>
		[JsonProperty(Required = Required.Always)]
		public string DisplayName { get; set; }

		/// <summary>
		/// Primary subscription key. If not specified during request key will be generated automatically.
		/// </summary>
		public string PrimaryKey { get; set; }

		/// <summary>
		/// Secondary subscription key. If not specified during request key will be generated automatically.
		/// </summary>
		public string SecondaryKey { get; set; }

		/// <summary>
		/// Initial subscription state. If no value is specified, subscription is created with Submitted state. 
		/// Possible states are * active – the subscription is active, * suspended – the subscription is blocked, and the subscriber cannot call any APIs of the product, 
		/// * submitted – the subscription request has been made by the developer, but has not yet been approved or rejected, 
		/// * rejected – the subscription request has been denied by an administrator, * cancelled – the subscription has been cancelled by the developer 
		/// or administrator, * expired – the subscription reached its expiration date and was deactivated. - suspended, active, expired, 
		/// submitted, rejected, cancelled
		/// </summary>
		public SubscriptionState? State { get; set; }

		/// <summary>
		/// Determines whether tracing can be enabled
		/// </summary>
		public bool? AllowTracing { get; set; }
	}
}
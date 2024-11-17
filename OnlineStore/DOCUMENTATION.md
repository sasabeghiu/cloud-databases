# API Documentation for OnlineStore

This document outlines the available REST API endpoints for interacting with the OnlineStore system.

## Base URL

`https://onlinestore-h0h3ckeab4h3dzau.northeurope-01.azurewebsites.net/api/`

## Endpoints

### Users

#### Register a user

- URL: `/Users`
- Method: `POST`
- Body: `{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "password": "securepassword"
}`
- Response: Status Code `201 Created`

#### Get user by ID

- URL: `/Users/{id}`
- Method: `GET`
- Response: `{
	"userId": 1,
	"firstName": "John",
	"lastName": "Doe",
	"email": "john.doe@example.com",
	"role": 2
}`

#### Get user by email

- URL: `/Users/email/{email}`
- Method: `GET`
- Response: `{
	"userId": 1,
	"firstName": "John",
	"lastName": "Doe",
	"email": "john.doe@example.com",
	"role": 2
}`

#### Update user role

- URL: `/Users/{id}/role`
- Method: `PUT`
- Body: `{
    "role": 1
}`
- Response: Status Code `204 No Content`

### Products

#### Create a product

- URL: `/Products`
- Method: `POST`
- Body: `{
    "name": "Product name",
    "description": "Product description",
    "price": 10.00,
    "imageUrl": "https://image.url"
}`
- Response: Status Code `201 Created`

#### Get all products

- URL: `/Products`
- Method: `GET`
- Response: `[
    {
        "productId": 1,
        "name": "Product 1",
        "description": "Product description",
        "image": "https://cdbstorageacc.blob.core.windows.net/product-images/cheese_14-Nov-24 06:19:45 PM.jpg",
        "price": 10.00,
        "stock": 100
    },
    {
        "productId": 2,
        "name": "Product 2",
        "description": "Product description",
        "image": "https://cdbstorageacc.blob.core.windows.net/product-images/cheese_14-Nov-24 06:19:45 PM.jpg",
        "price": 20.00,
        "stock": 50
    }
]`

#### Get product by ID

- URL: `/Products/{id}`
- Method: `GET`
- Response: `{
    "productId": 1,
    "name": "Product 1",
    "description": "Product description",
    "image": "https://cdbstorageacc.blob.core.windows.net/product-images/cheese_14-Nov-24 06:19:45 PM.jpg",
    "price": 10.00,
    "stock": 100
}`

#### Update product stock

- URL: `/Products/{id}/stock`
- Method: `PUT`
- Body: `{
    "quantity": 50
}`
- Response: Status Code `204 No Content`

#### Delete a product

- URL: `/Products/{id}`
- Method: `DELETE`
- Response: Status Code `204 No Content`

### Orders

#### Create an order

- URL: `/Orders`
- Method: `POST`
- Body: `{
    "userId": 1,
    "orderItems": [
        {
            "productId": 1,
            "quantity": 2
        },
        {
            "productId": 2,
            "quantity": 1
        }
    ]
}`
- Response: Status code `201 Created`

#### Get all orders

- URL: `/Orders?pageNumber=1&pageSize=10`
- Method: `GET`
- Response: `[
	{
		"orderId": 3,
		"userId": 3,
		"orderDate": "2024-11-14T18:24:42.3997638",
		"shippingDate": "2024-11-14T18:28:21.447",
		"status": "Shipped",
		"processedDuration": "0 days, 0 hours, 3 minutes, 39 seconds",
		"totalPrice": 18.00,
		"orderItems": [
			{
				"productId": 2,
				"quantity": 3
			},
			{
				"productId": 3,
				"quantity": 1
			}
		]
	},
	{
		"orderId": 4,
		"userId": 3,
		"orderDate": "2024-11-14T18:54:03.5853035",
		"shippingDate": "2024-11-14T19:00:30.012",
		"status": "Shipped",
		"processedDuration": "0 days, 0 hours, 6 minutes, 26 seconds",
		"totalPrice": 550.00,
		"orderItems": [
			{
				"productId": 2,
				"quantity": 110
			}
		]
	}
]`

#### Get order by ID

- URL: `/Orders/{id}`
- Method: `GET`
- Response: `[
    {
		"orderId": 4,
		"userId": 3,
		"orderDate": "2024-11-14T18:54:03.5853035",
		"shippingDate": "2024-11-14T19:00:30.012",
		"status": "Shipped",
		"processedDuration": "0 days, 0 hours, 6 minutes, 26 seconds",
		"totalPrice": 550.00,
		"orderItems": [
			{
				"productId": 2,
				"quantity": 110
			}
		]
	}
]`

#### Get orders by user ID

- URL: `/Orders/user/{userId}`
- Method: `GET`
- Response: `[
    {
		"orderId": 4,
		"userId": 3,
		"orderDate": "2024-11-14T18:54:03.5853035",
		"shippingDate": "2024-11-14T19:00:30.012",
		"status": "Shipped",
		"processedDuration": "0 days, 0 hours, 6 minutes, 26 seconds",
		"totalPrice": 550.00,
		"orderItems": [
			{
				"productId": 2,
				"quantity": 110
			}
		]
	}
]`

#### Update order status

- URL: `/Orders/{id}/status`
- Method: `PUT`
- Body: `{
    "status": "Shipped"
}`
- Response: Status code `204 No Content`

#### Update order shipping date

- URL: `/Orders/{id}/ship`
- Method: `PUT`
- Body: `{
  "shippingDate": "2024-11-14T12:00:00"
}`
- Response: Status code `204 No Content`

#### Delete a order

- URL: `/Orders/{id}`
- Method: `DELETE`
- Response: Status code `204 No Content`

### Reviews

#### Add a review

- URL: `/Reviews`
- Method: `POST`
- Body: `{
  "userId": 1,
  "productId": 1,
  "content": "review content",
  "rating": 5
}`
- Response: Status Code `201 Created`

#### Get review by ID

- URL: `/Reviews/{id}?userId={userId}`
- Method: `GET`
- Response: `{
  "reviewId": "76bf8926-b7d9-4a71-9cdc-0129da61abd5",
  "userId": 1,
  "productId": 1,
  "content": "review content",
  "rating": 5,
  "reviewDate": "2024-11-14T17:28:00.159303Z"
}`

#### Get reviews by product ID

- URL: `/Reviews/product/{productId}`
- Method: `GET`
- Response: `[
  {
    "reviewId": "2d70fccf-3874-4dd8-841d-3cdb60959ef5",
    "userId": 1,
    "productId": 1,
    "content": "review content",
    "rating": 5,
    "reviewDate": "2024-11-14T18:21:50.076421Z"
  },
  {
    "reviewId": "e78c8c18-5071-4a51-82ce-3725509974cb",
    "userId": 1,
    "productId": 2,
    "content": "review content",
    "rating": 3,
    "reviewDate": "2024-11-14T18:22:07.8776201Z"
  }
]`

#### Delete a review

- URL: `/Reviews/{id}?userId={userId}`
- Method: `DELETE`
- Response: Status Code `204 No Content`

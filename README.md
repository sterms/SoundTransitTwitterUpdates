# README #

Sound Transit Twitter Update, an Amazon Echo application that reads the last 12 hours of Sound Transit's Twitter Feed.

# OVERVIEW #

The Following is the order in which this will tentatively occur.

1. The User will request 'Flash Briefing' from an Echo device.
2. Echo will attempt to access a public json file in an Amazon S3 bucket.

1. CloudWatch events will be setup, hopefully in a variable time, to trigger a lambda function.
2. This Lambda function will pull down Twitter's latest updates from Sound Transit.
3. This Lambda will parse through the entries until it has the last 12 hours worth.
4. This Lambda will then format the text from the Tweets, scrubbing URLs and changing abbrviations and numeric pages to text better suited for text-to-speech.
5. This Lambda will then publish these entities, serialized to json, to a public file in an S3 bucket.

## TWITTER ##

### ACCESSING BEARER TOKEN ###

For more information, visit [The Twitter Api Documentation - Application-only authentication](https://developer.twitter.com/en/docs/basics/authentication/overview/application-only)

The _POST_ request to _https://api.twitter.com/oauth2/token_ must contain the following

* A Header, ```Authorization``` with the value of ```Basic <Base64Encoded(ConsumerKey:ComsumerSecret)>```
* A Header, ```Content-Type``` with the value of ```application/x-www-form-urlencoded;charset=UTF-8```
* A Body, with the Key-Value Pair ```grant_type, client_credentials```

_For Troubleshooting Errors, if the Twitter API returns code 99, make sure the concatenated string ConsumerKey:ConsumerSecret,
has been Base64 Encoded_

### ACCESSING TIMELINE DATA ###

For more information, visit [The Twitter Api Documentation - GET statuses/user_timeline ](https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-user_timeline.html)

The _GET_ request to _https://api.twitter.com/1.1/statuses/user_timeline.json_ features many Query String Parameters (see the link above for a full list), although ```screen_name```, ```count``` and ```since_id``` will probably prove to be the most useful. The request must contain the following

* A Header, ```Authorization``` with the value of ```Bearer XXXX```
* A Header, optional, ```Content-Type``` with the value of ```application/json```

## AWS ##

### CLOUD WATCH ###

Cloud Watch will Trigger the Lambda.

### LAMBDA ###

A Lambda needs to be setup to run the codebase. It will require the following Environment Variables:

- ENVIRONMENT (Production)
- TOKEN (Twitter Bearer Token)
- KEY_ID (Amazon IAM User Key)
- SECRET_KEY (Amazon IAM User Secret)

### IAM ###

An IAM role needs to be setup for Write Access to an S3 bucket.

### S3 ###

A S3 Bucket needs to be created for this application.

## .NET CORE 2.1 ##

### ENVIRONMENT VARIABLES ###

The codebase needs some Environment variables to function, these include:

- ENVIRONMENT (For setting up a Configuration)
- TOKEN (For the Twitter Service)
- KEY_ID (For the DynamoDbService)
- SECRET_KEY (For the DynamoDbService)

### AWS.SDK ###

The AWS.SDK package will need to be imported into this project, specifically to use it's access to S3.

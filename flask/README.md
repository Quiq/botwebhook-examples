# Basic Flask Webhook Example

## Usage: 


### Run server

```
$ pip install flask
$ cd botwebhook-examples/flask
$ flask run
```

### Enable public gateway

If you are running your bot locally, you will need to make sure it is visible to the public internet. Usually we
use something like [Ngrok](https://ngrok.com/ ) for that. It doesn't matter how you accomplish this, so long as your server is 
publically accessible. 

### Configure quiq bot

Configure your quiq bot to call your webhook via your publically accessible url. More details [here](https://knowledge.quiq.com/product-landings/bot-product-landing.html#call-webhook)


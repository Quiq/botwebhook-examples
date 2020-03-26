### Notes

- If you don't use ngrok, you can ignore this. This example utilizes [ngrok](https://ngrok.com/) to expose a local port over https so your Quiq site can access it. If you have a paid account for Ngrok, you can set the `ngrokSubdomain` environment variable to the ngrok subdomain you use, and the `ngrokAuthToken` to the auth token with your ngrok license. This is not required for the example, however without it, whenever you reset ngrok, you will need to update the webhook url in the Quiq Admin-UI Settings.
- This example also utilizes [nodemon](https://nodemon.io/) to automatically reload your server whenever you save your changes. This will not restart the ngrok server, as it does not need to be reset on every change.

### Instructions

1. After cloning the repository, run `npm install`. If you do not have node installed, you will first need to [download it](https://nodejs.org/en/)
2. run `npm start`. This will start the server

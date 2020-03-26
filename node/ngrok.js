const ngrok = require("ngrok");

(async () => {
  const ngrokUrl = await ngrok.connect({
    proto: "http",
    addr: 3002,
    subdomain: process.env.ngrokSubdomain,
    authtoken: process.env.ngrokAuthToken,
    region: "us"
  });

  console.log(`
*****  Put the following as your user's "Bot Webhook URL" -- ${ngrokUrl}  *****
*****  Put the following as your user's "Bot Webhook URL" -- ${ngrokUrl}  *****
*****  Put the following as your user's "Bot Webhook URL" -- ${ngrokUrl}  *****
*****  Put the following as your user's "Bot Webhook URL" -- ${ngrokUrl}  *****
*****  Put the following as your user's "Bot Webhook URL" -- ${ngrokUrl}  *****
  `);
  process.stdin.resume();
})();

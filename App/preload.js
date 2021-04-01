
// All of the Node.js APIs are available in the preload process.
// It has the same sandbox as a Chrome extension.
window.addEventListener('DOMContentLoaded', () => {
  const replaceText = (selector, text) => {
    const element = document.getElementById(selector)
    if (element) element.innerText = text
  }

  for (const type of ['chrome', 'node', 'electron']) {
    replaceText(`${type}-version`, process.versions[type])
  }
  const defaultGateway = require('default-gateway');

  const {gateway, interface} = defaultGateway.v4.sync()

  document.getElementById("con").innerHTML = gateway;

  const fs = require('fs');

  fs.readFile('baseurl.txt', (err, data) => {
    if (err) throw err;
  
    document.getElementById("baseUrl").innerHTML = data.toString();
  })

})

var jazzicon = require('@metamask/jazzicon');

function generateJazzicon(selector, diameter, address) {
    var addr = address.slice(2, 10);
    var seed = parseInt(addr, 16);
    var element = document.querySelector(selector);
    var jazz = jazzicon(diameter, seed);
    element.replaceChildren(jazz);
}

module.exports = {
    generateJazzicon: generateJazzicon
};
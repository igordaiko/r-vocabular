// Open a new search tab when the user clicks a context menu

const tldLocales = {
    'com.au': 'Australia',
    'com.br': 'Brazil',
    ca: 'Canada',
    cn: 'China',
    fr: 'France',
    it: 'Italy',
    'co.in': 'India',
    'co.jp': 'Japan',
    'com.ms': 'Mexico',
    ru: 'Russia',
    'co.za': 'South Africa',
    'co.uk': 'United Kingdom'
  };

chrome.runtime.onInstalled.addListener(async () => {

  });

chrome.contextMenus.create({
    id: "translate",
    title: `${chrome.i18n.getMessage("Translate")} '%s'`,
    contexts: ["selection"],
});


chrome.contextMenus.create({
    id: "shortcut",
    title: "ShortcutSetting",
    contexts: ["browser_action"],
});

chrome.contextMenus.create({
    id: "translate_page",
    title: "TranslatePage",
    contexts: ["page"],
});

chrome.contextMenus.onClicked.addListener((item, tab) => {
    const tld = item.menuItemId;
    const url = new URL(`https://google.${tld}/search`);
    url.searchParams.set('q', item.selectionText);
    chrome.tabs.create({ url: url.href, index: tab.index + 1 });
  });


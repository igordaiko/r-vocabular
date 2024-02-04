import { Parcel } from '@parcel/core';
import fetch from 'node-fetch'


const options = {
    watch: true,
    release: false
}

const bundler = new Parcel({
    entries: ['.'],
    defaultConfig: '@parcel/config-default',
    // shouldDisableCache: true,
    targets: ["src", "background"]
});

// const postcssPlugins = [

//     autoprefixer,
//     postcssNested,
//     postcssModule({

//         localsConvention: 'camelCaseOnly',

//         generateScopedName: !options.release

//             ? (name, filename, css) => `${path.relative(__dirname, filename).replace(/[\\\/.]/g, '_')}__${name}`

//             : (function() {

//                 const cache = {}

//                 const inc = incstr.idGenerator({
//                     alphabet: 'abcdefghijklmnopqrstuvwxyz0123456789_-'
//                 })

//                 const nextId = () => {
//                     const id = inc()
//                     return id === '-' ? inc() : id
//                 }

//                 return (name, filename, css) => {
//                     const key = `${filename}!${name}`
//                     return cache[key] || (cache[key] = `_${nextId()}`)
//                 }
//             })(),


//         getJSON: async (cssFileName, classes, _) => {

//             cssClasses.set(cssFileName, classes)

//             const exports = Object.keys(classes)
//                 .sort()
//                 .map(className => keywords.indexOf(className) < 0
//                     ? `export const ${className}: string${EOL}`
//                     : `declare const ${className}_: string${EOL}export { ${className}_ as ${className} }${EOL}`
//                 )
//                 .join('')

//             await fs.promises.writeFile(cssExportsFileName(cssFileName), exports)
//         }
//     })
// ]

// const postcssProcess = postcss(postcssPlugins)

// async function processCss(file, defsOnly) {

//     const content = await fs.promises.readFile(file)

//     const output = await postcssProcess.process(content, { from: file, to: file })

//     const classes = cssClasses.get(file)

//     const defaultExport = Object.entries(classes).map(([className, value]) => `    '${className}': '${value}'`).join(`,${EOL}`)

//     if (defsOnly) return

//     cssContent.set(cssContentFileName(file), output.css)

//     const importFile = cssContentFileName(path.relative(__dirname, file).replace(/\\/g, '/'))

//     return `import '@${importFile}'${EOL}export default {${EOL}${defaultExport}${EOL}}`
// }


if (options.watch) {

    await bundler.watch((err, event) => {

        if (err) {
            throw error;
        }


        if (event.type === 'buildSuccess') {

            let bundles = event.bundleGraph.getBundles();
            console.log(`✨ Built ${bundles.length} bundles in ${event.buildTime}ms!`);
        } else if (event.type === 'buildFailure') {

            console.log(event.diagnostics);
        }
    })
}

if (options.release) {
    await bundler.run()
}
import * as path from 'path';
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import basicSsl from '@vitejs/plugin-basic-ssl'

// https://vitejs.dev/config/
export default defineConfig({
    build: {
        lib: {
            entry: path.resolve(__dirname, 'src/components/index.ts'),
            name: 'CatfishUI',
            fileName: (format) => `catfish-ui.${format}.js`,
        },
        rollupOptions: {
            external: ['vue'],
            output: {
                // Provide global variables to use in the UMD build
                // Add external deps here
                globals: {
                    vue: 'Vue',
                },
            },
        },
    },
    plugins: [
        vue(),
        basicSsl()
    ],
    server: {
        port: 8080,
        https: true,
        hmr: {
            host: 'localhost',
            port: 8080,
            protocol: 'wss'
        }
    }
})

import * as path from 'path';
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import pinia from 'pinia'

// https://vitejs.dev/config/
export default defineConfig({
    define: { 'process.env.NODE_ENV': '"production"' },
    build: {
        lib: {
            entry: path.resolve(__dirname, 'src/components/index.ts'),
            name: 'Applets',//'CatfishUI'
            fileName: (format) => `applets.${format}.js`,
        },
        rollupOptions: {
            external: ['vue', 'pinia', 'router'],
            output: {
                // Provide global variables to use in the UMD build
                // Add external deps here
                globals: {
                    vue: 'Vue',
                    pinia: 'Pinia',
                    router: 'Router'
                },
            },
        }
    },
    plugins: [
        vue(),
        
    ],
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
        },
    },
    server: {
        port: 8080
    }
})


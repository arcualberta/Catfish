import * as path from 'path';
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import pinia from 'pinia'

// https://vitejs.dev/config/
export default defineConfig({
    build: {
        lib: {
            entry: path.resolve(__dirname, 'src/components/index.ts'),
            name: 'Applets',//'CatfishUI'
            fileName: (format) => `applets.${format}.js`,
        },
        rollupOptions: {
            external: ['vue', 'pinia'],
            output: {
                // Provide global variables to use in the UMD build
                // Add external deps here
                globals: {
                    vue: 'Vue',
                    pinia: 'Pinia',
                    
                },
            },
        },
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
function importToCDN(arg0: { modules: { name: string; var: string; path: string; }[]; }): import("vite").PluginOption {
    throw new Error('Function not implemented.');
}


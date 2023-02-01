import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";

import { LoginResult } from '../models';
import jwt_decode from "jwt-decode";
import {get, set, useStorage} from '@vueuse/core'
import { computed } from 'vue';

export const useLoginStore = defineStore('LoginStore', {
    state: () => ({
        authorizationApiRoot: null as string | null,
        loginResult: {
            get: ()=>{
                let loginResultStr = localStorage.getItem("catfishLoginResult")
                return eval(loginResultStr as string);
            },
            set:(val: LoginResult)=> {
                localStorage.setItem("catfishLoginResult", JSON.stringify(val))
            }
        }, //null as LoginResult | null,
        jwtToken: {
            get: ()=>  localStorage.getItem("catfishJwtToken"),
            set:(val: string)=> localStorage.setItem("catfishJwtToken", val)
        }
        
    }),
    actions: {
        authorize(jwt: string) {
            if (!jwt) {
                console.error("JWT token is null.")
                return;
            }

            if (!this.authorizationApiRoot) {
                console.error('Authorization service root is not specified.')
                return;
            }
            const api = this.authorizationApiRoot?.replace(/\/+$/, '') + '/api/GoogleIdentity'
            fetch(api,
                {
                    body: JSON.stringify(jwt),
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json'
                    },
                })
                .then(response => response.text())
                .then(data => {
                   
                        this.jwtToken = data as string;
                        //this.loginResult = data as LoginResult;
                        let loginRes = jwt_decode(data) as LoginResult;
                        loginRes.success = true;
                        this.loginResult = loginRes;
                        //this.loginResult.success=true;
                        //console.log(JSON.stringify(this.loginResult));
                  
                })
                .catch((error) => {
                    this.loginResult = {} as LoginResult;
                    console.error('User authorization failed: ', error)
                });
        },
       
    }
    
});
import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";

import { LoginResult } from '../models';
import jwt_decode from "jwt-decode";

import { computed } from 'vue';

export const useLoginStore = defineStore('LoginStore', {
    state: () => ({
        authorizationApiRoot: null as string | null,
        loginResult: {
            get: ()=>{
                if (localStorage.getItem("catfishLoginResult") === null) {
                    return {} as LoginResult;
                }
                let loginResultStr = localStorage.getItem("catfishLoginResult")
                return JSON.parse(loginResultStr as string);
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
                   
                       // this.jwtToken = data as string;
                        this.jwtToken.set(data as string);
                        //this.loginResult = data as LoginResult;
                        let loginRes = jwt_decode(data) as LoginResult;
                        loginRes.success = true;
                        this.loginResult.set(loginRes);
                        //this.loginResult.success=true;
                        //console.log(JSON.stringify(this.loginResult));
                       window.location.href="/";
                  
                })
                .catch((error) => {
                    this.loginResult.set({} as LoginResult);
                    console.error('User authorization failed: ', error)
                });
        },
       
    }
    
});
import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";

import { LoginResult } from '../models';
import jwt_decode from "jwt-decode";

export const useLoginStore = defineStore('LoginStore', {
    state: () => ({
        authorizationApiRoot: null as string | null,
        loginResult: null as LoginResult | null,
        jwtToken: null as string | null
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
                        this.loginResult =jwt_decode(data) as LoginResult;
                        this.loginResult.success=true;
                        //console.log(JSON.stringify(this.loginResult));
                  
                })
                .catch((error) => {
                    this.loginResult = {} as LoginResult;
                    console.error('User authorization failed: ', error)
                });
        },
    }
});
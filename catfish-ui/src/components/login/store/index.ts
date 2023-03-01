import { defineStore } from 'pinia';
import { Guid } from "guid-typescript";

import { LoginResult } from '../models';
import jwt_decode from "jwt-decode";

import { computed } from 'vue';
import router from '@/router';

export const useLoginStore = defineStore('LoginStore', {
    state: () => ({
        authorizationApiRoot: null as string | null,

        //Creating loginResult property with it's setter and getter overridden to
        //store and retrieve value through local storage.
        get loginResult() {
            if (localStorage.getItem("catfishLoginResult") === null) {
                return {} as LoginResult;
            }
            else {
                return JSON.parse(localStorage.getItem("catfishLoginResult") as string) as LoginResult;
            }
        },
        set loginResult(val: LoginResult) { localStorage.setItem("catfishLoginResult", JSON.stringify(val)) },

        //Creating jwtToken property with it's setter and getter overridden to
        //store and retrieve value through local storage.
        get jwtToken() { return localStorage.getItem("catfishJwtToken") as string },
        set jwtToken(val: string) { localStorage.setItem("catfishJwtToken", val) }
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

                    let loginRes = jwt_decode(data) as LoginResult;
                    loginRes.success = true;
                    //this.loginResult.set(loginRes);
                    this.loginResult = loginRes;

                    //window.location.href="/";
                    router.push("/");

                })
                .catch((error) => {
                    this.loginResult = {} as LoginResult;
                    console.error('User authorization failed: ', error)
                });
        },

    }

});
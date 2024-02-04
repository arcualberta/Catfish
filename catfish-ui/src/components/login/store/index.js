import { defineStore } from 'pinia';
import jwt_decode from "jwt-decode";
import router from '@/router';
export const useLoginStore = defineStore('LoginStore', {
    state: () => ({
        authorizationApiRoot: null,
        //Creating loginResult property with it's setter and getter overridden to
        //store and retrieve value through local storage.
        get loginResult() {
            if (localStorage.getItem("catfishLoginResult") === null) {
                return {};
            }
            else {
                return JSON.parse(localStorage.getItem("catfishLoginResult"));
            }
        },
        set loginResult(val) { localStorage.setItem("catfishLoginResult", JSON.stringify(val)); },
        //Creating jwtToken property with it's setter and getter overridden to
        //store and retrieve value through local storage.
        get jwtToken() { return localStorage.getItem("catfishJwtToken"); },
        set jwtToken(val) { localStorage.setItem("catfishJwtToken", val); }
    }),
    actions: {
        authorize(jwt) {
            if (!jwt) {
                console.error("JWT token is null.");
                return;
            }
            if (!this.authorizationApiRoot) {
                console.error('Authorization service root is not specified.');
                return;
            }
            const api = this.authorizationApiRoot?.replace(/\/+$/, '') + '/api/GoogleIdentity';
            fetch(api, {
                body: JSON.stringify(jwt),
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
            })
                .then(response => response.text())
                .then(data => {
                this.jwtToken = data;
                let loginRes = jwt_decode(data);
                loginRes.success = true;
                //this.loginResult.set(loginRes);
                this.loginResult = loginRes;
                //window.location.href="/";
                router.push("/");
            })
                .catch((error) => {
                this.loginResult = {};
                console.error('User authorization failed: ', error);
            });
        },
    }
});
//# sourceMappingURL=index.js.map
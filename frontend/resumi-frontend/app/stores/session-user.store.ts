import type { ReadUserModel } from "~/data/api/read-user-model"

export type SessionUserStore = {
    sessionUser?: ReadUserModel
    isAuthenticated: boolean
}

export const useSessionUserStore = defineStore('sessionUser', {
    state: (): SessionUserStore => {
        return {
            sessionUser: undefined,
            isAuthenticated: false
        }
    },
    actions: {
        setSessionUser(user?: ReadUserModel) {
            this.sessionUser = user
            this.isAuthenticated = user !== undefined
        }
    }
})
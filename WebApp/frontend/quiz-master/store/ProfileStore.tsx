import { create } from "zustand";

export interface IAccount {
    id: Number;
    lastName: string | null;
    firstName: string | null;
    email: string;
    userName: string;
    activeData: boolean;
    dateCreated: Date;
    dateUpdated: Date | null;
    updatedByUser: Number | null;
}

export class UserAccount implements IAccount {
    id = 0;
    lastName = null;
    firstName = null;
    email = "";
    userName = "";
    activeData = false;
    dateCreated = new Date();
    dateUpdated = null;
    updatedByUser = null;
    constructor() {}
    parse(d: IAccount) {
        let copyOfThis = this as IAccount;
        for (let key in copyOfThis) {
            if (d[key as keyof IAccount] !== null) {
                copyOfThis[key as keyof IAccount] = d[key as keyof IAccount];
            }
        }
        return copyOfThis;
    }
}

interface IAccountStore {
    roles: Array<string>;
    account: IAccount | null | undefined;
    setAccount: (accountData: IAccount) => void;
    getAccount: () => IAccount | null | undefined;
    setRoles: (roles: Array<string>) => void;
    getRoles: () => Array<string>;
}

export const useAccountStore = create<IAccountStore>((set, get) => ({
    roles: [""],
    account: null,
    setAccount: (accountData: IAccount) => {
        set({ account: accountData });
    },
    getAccount: () => {
        return get().account;
    },
    setRoles: (roles: Array<string>) => {
        set({ roles });
    },
    getRoles: () => {
        return get().roles;
    },
}));

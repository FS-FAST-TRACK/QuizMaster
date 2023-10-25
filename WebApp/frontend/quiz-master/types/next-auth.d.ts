import { Session } from "next-auth";
import { JWT } from "next-auth/jwt";
export interface User {
	name: string;
	email: string;
	username: string;
	role: string;
}
declare module "next-auth" {
	interface Session {
		user: User;
	}
}

declare module "next-auth/jwt" {
	interface JWT {
		user: User;
	}
}

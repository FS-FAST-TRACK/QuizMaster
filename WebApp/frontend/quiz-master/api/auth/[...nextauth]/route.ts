import jwtDecode from "jwt-decode";
import { ISODateString } from "next-auth";
import NextAuth from "next-auth/next";
import CredentialsProvider from "next-auth/providers/credentials";
import { Session } from "next-auth";
import { JWT } from "next-auth/jwt";
import { User } from "@/types/next-auth";
interface DataToken {
	token: string;
	nbf: ISODateString;
	exp: ISODateString;
	iat: ISODateString;
}

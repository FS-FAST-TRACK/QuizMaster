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
const authOptions = {
    providers: [
        CredentialsProvider({
            // The name to display on the sign in form (e.g. 'Sign in with...')
            name: "credentials",
            // The credentials is used to generate a suitable form on the sign in page.
            // You can specify whatever fields you are expecting to be submitted.
            // e.g. domain, username, password, 2FA token, etc.
            // You can pass any HTML attribute to the <input> tag through the object.
            credentials: {
                jwt: {},
            },
            async authorize(credentials, req) {
                try {
                    // Guard of null jwt token
                    if (!credentials?.jwt) {
                        return null;
                    }

                    // Decode jwt token
                    const decode: DataToken = jwtDecode(credentials?.jwt);
                    const parse = JSON.parse(decode.token);
                    const parsed = parse.UserData;

                    // Assign parsed user to user
                    const user = {
                        name: `${parsed.FirstName} ${parsed.LastName}`,
                        email: parsed.Email,
                        username: parsed.UserName,
                        role: parse.Roles[0].Name,
                    };
                    return user as any;
                } catch (error) {
                    console.error(error);
                }
            },
        }),
    ],
    pages: {
        signIn: `${process.env.BASE_URL ?? 'localhost:3000'}/auth/login`,
        signOut: `${process.env.BASE_URL ?? 'localhost:3000'}/auth/signout`,
    },

    secret:
        process.env.NEXTAUTH_SECRET ??
        process.env.SECRET ??
        "04e9d3fbe3c8fbfb7e5f89892751f8c5",
    callbacks: {
        jwt: async ({ token, user }: { token: JWT; user: User }) => {
            user && (token.user = user);
            return token;
        },
        async signIn({ user }: { user: User }) {
            return user;
        },
        async session({ session, token }: { session: Session; token: JWT }) {
            session.user = token.user;

            return session;
        },
    },

    site: process.env.BASE_URL
};

const handler = NextAuth(authOptions as any);

export { handler as GET, handler as POST };

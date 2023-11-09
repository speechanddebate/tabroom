<span class="login">
	<a href="#login-box" class="login-window">Login</a>
	<div id="login-box" class="login-popup">

		<div class="padbottomless marbottomless">
			<h6 class="fivesixths semibold leftalign">
				<span class="quarterspacer"></span>
				Login to Tabroom
			</h6>

			<a href="#top" class="sixth close fa fa-times-circle fa-lg bluetext">
			</a>
		</div>

		<form
			name   = "login"
			action = "/user/login/login_save.mhtml"
			method = "post"
			class  = "signin"
		>
				<div class="full centeralign">
					<span class="ninetenths nospace">
						<input
							type         = "text"
							id           = "username"
							name         = "username"
							value        = ""
							autocomplete = "on"
							placeholder  = "Email address"
						>
					</span>
				</div>

				<div class="full centeralign">
					<span class="ninetenths nospace">
						<input
							id          = "password"
							name        = "password"
							type        = "password"
							value       = ""
							placeholder = "Password"
						>
					</span>
				</div>

				<div class="full centeralign">
					<span class="ninetenths nospace">
						<input
							type  = "submit"
							value = "Login"
							class = "full"
						>
					</span>
				</div>
		</form>

		<a
			class = "redtext italic martopmore rightalign full martopmore link-underline biggish"
			href  = "/user/login/forgot.mhtml"

		>Forgot your Password?</a>
	</div>
</span>

<span class="signup">
	<a tabindex="-1" href="/user/login/new_user.mhtml">
		Sign Up
	</a>
</span>

<style>
	.login-popup {
		display                   : none;
		position                  : absolute;
		background                : var(--light-gray);
		padding                   : 10px;
		margin-top                : 4px;
		border                    : 4px solid var(--dark-blue);
		z-index                   : 99999;
		box-shadow                : 0 0 20px var(--medium-gray);
		border-radius             : 3px;
		width                     : 256px;
	}

	a.redtext {
		color           : var(--dark-red);
		text-decoration : underline;
	}

	a.close {
		display      : block;
		float        : right;
		color        : var(--darker-blue);
		padding      : 8px;
		margin-top   : -2px;
		margin-right : -2px;
		font-size    : 1.4em;
	}

	input[type="text"],
	input[type="password"] {
		border-bottom : 1px solid var(--dark-gray);
		border-left   : 1px solid var(--darker-gray);
		border-right  : 1px solid var(--dark-gray);
		border-top    : 1px solid var(--darker-gray);
		color         : var(--darker-blue);
		border-radius : 4px;
		font          : 15px "Inconsolata";
		padding       : 8px 8px 4px;
		margin-top    : 6px;
		margin-bottom : 6px;
		width         : 84%;
	}

	input[type="submit"] {
		background-color : var(--darker-blue);
		font             : 16px "Inconsolata";
		font-weight      : 600;
		color            : var(--background-white);
		padding-top      : 8px;
		padding-bottom   : 8px;
		padding-right    : 16px;
		margin-top       : 8px;
		text-align       : right;
	}
</style>

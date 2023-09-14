using Microsoft.Extensions.Primitives;

namespace todo_list.Utils;

public static class Permissions
{
	public static bool CanUpdate(StringValues headerAuth, Guid itemId)
	{
		var jwtToken = TokenUtils.ReadToken(headerAuth);

		var tokenUserRole = TokenUtils.GetUserRole(jwtToken);
		var tokenUserId = TokenUtils.GetUserId(jwtToken);
		var stringGuid = itemId.ToString();
		var isSameUser =
			!string.IsNullOrWhiteSpace(tokenUserId) && !string.IsNullOrWhiteSpace(stringGuid) && tokenUserId == stringGuid;

		var isAdmin = string.Equals(tokenUserRole, "Administrator", StringComparison.OrdinalIgnoreCase);

		return isAdmin || isSameUser;
	}
}

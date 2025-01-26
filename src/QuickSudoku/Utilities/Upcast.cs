// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Utilities;

static class Upcast
{
    public static T To<T>(T value)
        => value;
}

package swarm.gui;

//
// SwarmChemistry.java
// Public Release Version 1.2.0
//
// 2006-2009 (c) Copyright by Hiroki Sayama. All rights reserved.
//
// Send any correspondences to:
//   Hiroki Sayama, D.Sc.
//   Assistant Professor, Department of Bioengineering
//   Binghamton University, State University of New York
//   P.O. Box 6000, Binghamton, NY 13902-6000, USA
//   Tel: +1-607-777-4439  Fax: +1-607-777-5780
//   Email: sayama@binghamton.edu
//
// For more information about this software, see:
//   http://bingweb.binghamton.edu/~sayama/SwarmChemistry/
//

import java.awt.*;
import java.applet.Applet;

public class SwarmChemistry extends Applet {
    public static void main(String args[]) {
	boolean recipeIsGiven = false;
	int n = 6;
	SwarmChemistryEnvironment master;

	if (args.length > 0) {
	    try {
		n = Integer.parseInt(args[0]);
	    }
	    catch(NumberFormatException e) {
		n = 1;
		recipeIsGiven = true;
	    }
	    if (n < 1) n = 1;
	}

	if (recipeIsGiven)
	    master = new SwarmChemistryEnvironment(false, args[0]);
	else
	    master = new SwarmChemistryEnvironment(false, n);
    }

    public void init() {
	SwarmChemistryEnvironment master = new SwarmChemistryEnvironment(true, 6);
    }
}
